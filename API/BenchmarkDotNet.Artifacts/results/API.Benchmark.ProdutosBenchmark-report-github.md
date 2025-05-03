```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3775)
Unknown processor
.NET SDK 9.0.203
  [Host]     : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2


```
| Method                 | Mean     | Error   | StdDev  | Allocated |
|----------------------- |---------:|--------:|--------:|----------:|
| GetProdutosIEnumerable | 166.0 μs | 3.01 μs | 4.87 μs |   6.76 KB |
