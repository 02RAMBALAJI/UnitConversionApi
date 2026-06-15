<<<<<<< HEAD
# Unit Conversion API

A production-ready **ASP.NET Core 8** RESTful Web API for converting numerical values between different units of measurement.

## Features

- Convert between **Length**, **Temperature**, and **Weight/Mass** units
- Both `POST` (JSON body) and `GET` (query string) conversion endpoints
- Swagger / OpenAPI UI for interactive exploration
- Uniform `{ success, data, error }` response envelope
- Global exception handling middleware
- Fully unit-tested with xUnit + FluentAssertions

---

## Supported Units

### Length
| ID | Name | Symbol |
|----|------|--------|
| `meter` | Meter | m |
| `kilometer` | Kilometer | km |
| `centimeter` | Centimeter | cm |
| `millimeter` | Millimeter | mm |
| `mile` | Mile | mi |
| `yard` | Yard | yd |
| `foot` | Foot | ft |
| `inch` | Inch | in |
| `nautical_mile` | Nautical Mile | nmi |

### Temperature
| ID | Name | Symbol |
|----|------|--------|
| `celsius` | Celsius | °C |
| `fahrenheit` | Fahrenheit | °F |
| `kelvin` | Kelvin | K |

### Weight / Mass
| ID | Name | Symbol |
|----|------|--------|
| `kilogram` | Kilogram | kg |
| `gram` | Gram | g |
| `milligram` | Milligram | mg |
| `microgram` | Microgram | µg |
| `pound` | Pound | lb |
| `ounce` | Ounce | oz |
| `ton` | Metric Ton | t |
| `stone` | Stone | st |

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

Verify your installation:

```bash
dotnet --version   # Should print 8.x.x
```

---

## Running Locally

```bash
# 1. Clone the repository
git clone <repository-url>
cd UnitConversionApi

# 2. Restore NuGet packages
dotnet restore

# 3. Run the API
dotnet run --project src/UnitConversionApi

# 4. Open Swagger UI in your browser
#    http://localhost:5000/swagger
```

The API starts on **http://localhost:5000** (HTTP) and **https://localhost:5001** (HTTPS).

---

## Running Tests

```bash
dotnet test
```

To run with detailed output:

```bash
dotnet test --logger "console;verbosity=detailed"
```

---

## API Reference

### Convert a Value

#### POST `/api/conversions`

**Request body:**
```json
{
  "value": 100,
  "fromUnit": "meter",
  "toUnit": "foot"
}
```

**Response `200 OK`:**
```json
{
  "success": true,
  "data": {
    "inputValue": 100,
    "fromUnit": "Meter",
    "outputValue": 328.0839895013,
    "toUnit": "Foot",
    "category": "length"
  },
  "error": null
}
```

#### GET `/api/conversions?value=0&from=celsius&to=fahrenheit`

Same response shape as POST. Convenient for quick browser or curl tests.

**Example:**
```bash
curl "http://localhost:5000/api/conversions?value=100&from=celsius&to=fahrenheit"
```

---

### Discover Units

| Endpoint | Description |
|----------|-------------|
| `GET /api/units` | All available units |
| `GET /api/units/categories` | Units grouped by category |
| `GET /api/units/{id}` | A specific unit by ID |

---

### Error Responses

All errors follow the same envelope:

```json
{
  "success": false,
  "data": null,
  "error": "Unknown unit: 'lightyear'. Call GET /api/units for a list of valid unit IDs."
}
```

| HTTP Status | Cause |
|-------------|-------|
| `400 Bad Request` | Unknown unit ID or malformed request |
| `404 Not Found` | Unit not found (GET /api/units/{id}) |
| `422 Unprocessable Entity` | Cross-category conversion attempt |
| `500 Internal Server Error` | Unexpected server error |

---

## Project Structure

```
UnitConversionApi/
├── README.md
├── UnitConversionApi.sln
├── src/
│   └── UnitConversionApi/
│       ├── Controllers/
│       │   ├── ConversionsController.cs   # POST & GET conversion endpoints
│       │   └── UnitsController.cs         # Unit discovery endpoints
│       ├── Data/
│       │   └── UnitDefinitions.cs         # Hardcoded unit data & conversion factors
│       ├── Middleware/
│       │   └── ExceptionHandlingMiddleware.cs
│       ├── Models/
│       │   ├── ApiResponse.cs             # Uniform response envelope
│       │   ├── ConversionRequest.cs
│       │   ├── ConversionResponse.cs
│       │   ├── Unit.cs
│       │   └── UnitCategory.cs
│       ├── Services/
│       │   ├── IUnitConversionService.cs
│       │   ├── UnitConversionService.cs   # Core conversion logic
│       │   ├── IUnitRegistryService.cs
│       │   └── UnitRegistryService.cs     # In-memory unit registry
│       └── Program.cs
└── tests/
    └── UnitConversionApi.Tests/
        └── Services/
            └── UnitConversionServiceTests.cs
```

---

## Design Decisions & Trade-offs

### 1. Base-Unit Conversion Strategy
For linear categories (length, weight), all conversions flow through a common base unit (meter, kilogram):

```
value_from × factor_from → base_unit → ÷ factor_to → value_to
```

**Benefit:** Adding a new unit only requires one number (its factor to the base unit) rather than mapping it against every existing unit.  
**Trade-off:** Tiny floating-point rounding may occur at extreme precision; acceptable for typical use.

### 2. Temperature is Special-Cased
Temperature uses affine formulas (scale + offset) rather than a simple multiplication factor. The service converts to Celsius as an intermediate step.

### 3. Hardcoded Data in `UnitDefinitions.cs`
Conversion factors are hardcoded per requirements. The data is isolated behind the `IUnitRegistryService` interface — swapping to a database or config-file source requires only a new implementation, no changes to controllers or conversion logic.

### 4. Uniform API Response Envelope
Every response (success or error) has the same shape `{ success, data, error }`. This makes client-side handling predictable and removes the need to inspect HTTP status alone.

### 5. Both POST and GET for Conversions
POST (JSON body) is idiomatic REST. GET (query params) is included for browser-friendly testing and simple integrations. Both share the same service layer.

### 6. Singleton Registry, Scoped Converter
`UnitRegistryService` is singleton — the dictionary never changes. `UnitConversionService` is scoped to allow easy extension with per-request state (e.g. user-defined units) in future iterations.

### 7. Extensibility Path
To scale to hundreds of unit types:
- Add new entries to `UnitDefinitions.cs` (or replace with a DB-backed `IUnitRegistryService`)
- Add a new `case` in the `UnitConversionService` switch for any category with non-linear formulas
- No controller changes required
=======
# UnitConversionApi
>>>>>>> 6c91a0f654eb4bdade903a4930c793548a7fd17b
