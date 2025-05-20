```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Unknown processor
.NET SDK 9.0.204
  [Host]   : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2
  ShortRun : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method                 | Mean     | Error    | StdDev  | Allocated |
|----------------------- |---------:|---------:|--------:|----------:|
| GetProdutosIEnumerable | 129.4 μs | 99.91 μs | 5.48 μs |   5.98 KB |
