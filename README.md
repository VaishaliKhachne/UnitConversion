# Unit Conversion API

A REST API built with ASP.NET Core that converts numeric values between
units of measurement across three categories: **length**, **weight**, and
**temperature**.

---

## How to get and run this project

### 1. Download the code from GitHub

1. Go to the repository page: `https://github.com/<your-username>/UnitConversion`
2. Click the green **Code** button
3. Click **Download ZIP**
4. Extract the ZIP to a folder on your machine

*(Alternatively, if you have Git installed: `git clone https://github.com/VaishaliKhachne/UnitConversion.git`)*

### 2. Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed
- Visual Studio 2022 (Community edition is free) with the **ASP.NET and web development** workload

### 3. Open and run in Visual Studio

1. Launch **Visual Studio 2022**
2. **File → Open → Project/Solution**
3. Select `UnitConversion.sln` from the extracted folder
4. In **Solution Explorer**, right-click the **UnitConversion.Api** project → **Set as Startup Project**
5. Click the green **▶ Run** button (or press `F5`)
6. Wait for the console window to open — it will print something like:
```
   Now listening on: https://localhost:<portnumer>
```
7. Copy that `https://` URL from the console

### 4. Open Swagger

1. In your browser, go to:
```
   https://localhost:<portnumber>/swagger
```
   (replace the port with whatever your console printed)
2. You'll see two endpoints listed: `GET /api/v1/units` and `POST /api/v1/conversions`

### 5. Check supported units

1. Expand **GET /api/v1/units**
2. Click **Try it out** → **Execute**
3. The response body lists every supported unit code, its display name, and category — use these exact codes (e.g. `m`, `ft`, `kg`, `c`) when calling the conversion endpoint

### 6. Try a conversion

1. Expand **POST /api/v1/conversions**
2. Click **Try it out**
3. Replace the example body with:
```json
   { "from": "c", "to": "f", "value": 100 }
```
4. Click **Execute** — expect a `200` response with `convertedValue: 212`

---

## Design decisions

**Layered architecture (Domain → Application → Api)**
The solution is split into three projects with a strict one-way dependency
flow: `Domain` has no dependencies, `Application` depends only on `Domain`,
and `Api` depends only on `Application`. This separation of concerns means
business logic (conversion rules) is fully decoupled from HTTP concerns
(controllers, DTOs) — the conversion engine could be reused in a console
app, a background job, or a different API entirely without any changes.

**Strategy pattern for conversions (`IUnitConverter`)**
Rather than one large `switch` statement handling every unit pairing,
each category (`LengthConverter`, `WeightConverter`, `TemperatureConverter`)
implements a shared `IUnitConverter` interface. This is a direct
application of the **Open/Closed Principle** — adding a new category
(e.g. volume, area) in the future means writing one new class and
registering it in DI; no existing, already-tested code needs to change.

**Singleton lifetime for converters (`AddSingleton`)**
```csharp
builder.Services.AddSingleton<IUnitConverter, LengthConverter>();
```
Converters are registered as **singletons** rather than scoped/transient
because they are completely stateless — they hold only static conversion
factors and contain no per-request or per-user data. Reusing one instance
across the application's lifetime avoids unnecessary object allocation on
every request, which matters as this scales toward "hundreds of units" and
higher request volume. This was a deliberate performance/design decision,
not a default.

**Centralized exception handling (`IExceptionHandler`)**
Domain errors (`UnknownUnitException`, `IncompatibleUnitsException`) are
thrown from deep inside the service layer but never leak as raw stack
traces to the caller. A single `DomainExceptionHandler` intercepts them at
the pipeline level and converts them into consistent, clean HTTP responses
(`400` with a `ProblemDetails` body). This keeps error-handling logic out
of every controller action — controllers stay focused purely on
request/response shape, not on knowing which exceptions mean what status
code.

**Hardcoded data, isolated behind one class (`UnitCatalog`)**
Per the challenge requirements, all units and conversion factors are
hardcoded for this version. However, they are deliberately isolated inside
a single static class rather than scattered across the codebase. This was
a conscious trade-off: in a production system supporting hundreds of
units, this data would move to a database with a repository/service layer
behind an interface (e.g. `IUnitRepository`) — and because nothing outside
`UnitCatalog` knows *how* the data is stored, making that change later
would not require touching controllers, converters, or the conversion
service at all.

**OOP principles applied throughout**
- **Encapsulation** — `UnitCatalog`'s internal storage (`FrozenDictionary`,
  `List`) is private; callers only interact through `Find()` and
  `ByCategory()` methods, never the raw collections.
- **Abstraction** — controllers and the conversion service depend only on
  interfaces (`IUnitConverter`, `IConversionService`), never concrete
  implementations, enabling easy substitution and unit testing with mocks.
- **Inheritance** — `LengthConverter` and `WeightConverter` both extend a
  shared `LinearUnitConverterBase`, since their conversion math (convert to
  a base unit, then to the target) is identical; only the factor tables
  differ.
- **Polymorphism** — `ConversionService` calls `Convert()` on whichever
  `IUnitConverter` matches the requested category at runtime, without
  needing to know or check which concrete converter class it's talking to.

**Validation via Data Annotations + `[ApiController]`**
Request validation (`from`/`to` required) uses built-in Data Annotations
rather than a third-party validation library. Because the controller is
decorated with `[ApiController]`, invalid requests are automatically
rejected with a `400` before the action method even executes — no manual
validation code needed. This keeps the dependency footprint minimal for
validation rules this simple; a more complex rule set (cross-field
conditions, async database checks) would justify introducing a dedicated
validation library.

**`decimal` over `double` for all conversion math**
Financial and measurement-style calculations use `decimal` throughout to
avoid floating-point rounding drift that `double` can introduce — important
since converted values are returned directly to callers who may act on
them precisely.

---

## Troubleshooting

**Requirement: .NET 8 SDK**
This project targets `net8.0`. Before running, confirm your machine has
the correct SDK installed:
```bash
dotnet --list-sdks
```
You should see an entry starting with `8.`. If not, install it from
https://dotnet.microsoft.com/download/dotnet/8.0 before opening the
solution.

**Build fails immediately after opening the solution**
- Close Visual Studio
- Delete every `bin` and `obj` folder inside `src/*` and `tests/*`
- Reopen the solution and use **Build → Rebuild Solution**

**`ReflectionTypeLoadException` on startup**
This typically indicates a NuGet package version mismatch or a stale
build. Fix:
1. Build menu → **Clean Solution**
2. Delete `bin`/`obj` folders as above
3. Build menu → **Rebuild Solution**
4. Run again

**Swagger page doesn't load / shows a blank page**
- Confirm you copied the exact URL and port printed in the console
  window — it changes between runs sometimes
- Confirm you're running in the **Development** environment (default when
  launching via Visual Studio's `F5`); Swagger is disabled outside
  Development by design

**Browser shows an SSL/certificate warning on `https://localhost`**
This is expected for local development certificates. Either accept/trust
the certificate when prompted, or use the `http://` URL variant printed
in the same console output instead.

**Port already in use**
If another process is already using the printed port, stop any previously
running instance of the app (check for a lingering `UnitConversion.Api`
process), or simply re-run — ASP.NET Core will select an open port on
the next attempt if configured to do so.

---

## Project structure

```
UnitConversion.sln
├── src/
│   ├── UnitConversion.Domain/          # Enums, models, exceptions — no dependencies
│   ├── UnitConversion.Application/     # Conversion logic, converters, hardcoded catalog
│   └── UnitConversion.Api/             # Controllers, DTOs, Program.cs, Swagger
└── tests/
    ├── UnitConversion.Application.Tests/     # Unit tests for conversion logic
    └── UnitConversion.Api.IntegrationTests/  # HTTP-level tests via WebApplicationFactory
```
