## Implementation


Implemented endpoint `/convert` to convert raw binary stream into `JPEG` response stream or throw Error.


## Details

1. `/convert` uses [Magick.NET](https://github.com/dlemstra/Magick.NET) a .NET wrapper around [Image Migick](https://imagemagick.org/index.php). Choose this library mainly cause of it's 100+ different image formats support. Thought it was mostly on 2nd position in performance to `ImageSharp` in cross-platform, the differences very negligible [ref](https://devblogs.microsoft.com/dotnet/net-core-image-processing/).

2. Setup `Docker` support using VS Generated template and integrated the service with `docker-compose` on port `5000`

3. Used [OneOf](https://github.com/mcintyre321/OneOf/) package as Control Flow technique to avoid using `null`/ throwing `exceptions`.

4. Tested with `xUnit` and `NSubstitute`

## Performance

Regarding Performance main focus point was trying to get processing of `big.jpg` improved.

-- Implemented initializing of `Magic.NET` on start of the server, to reduce the impact of Cold Start. This results in following metrics:

(results for `big.jpg` as reported by OpenTelemetry using JAEGER UI)

Without pre-initialization: 3.46s ~ 4.31s
With initialization: 1.77s ~ 2.02s

-- Researched using `Magick.NET` with `OpenMP`, thought it worked significantly improved on `Windows`, it brought down processing time for `big.jpg` under a `970ms` (without pre-initialization).


 on `Linux (Debian)` couldn't get it to work, it couldn't link `Magick.Native-Q8-OpenMP-x64.dll.so` and it's static dependencies, which had to be manually built on `Linux` which I managed on `WSL`, but it never loaded properly.

-- Also researched relatively bad metrics on linux (Docker), it was being caused by `mem_limit: 512m`. Turns out `Magick.NET` has internal memory limit, which when it exceeds, it starts using file-system to cache pixel data, which significantly reduced the performance.
