``` ini

BenchmarkDotNet=v0.10.11, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.192)
Processor=Intel Core i5-7200U CPU 2.50GHz (Kaby Lake), ProcessorCount=4
Frequency=2648443 Hz, Resolution=377.5803 ns, Timer=TSC
  [Host]     : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2600.0
  DefaultJob : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2600.0


```
|      Method |      Mean |     Error |    StdDev |   Gen 0 | Allocated |
|------------ |----------:|----------:|----------:|--------:|----------:|
| TestAddsOld | 133.41 us | 2.6005 us | 3.9712 us | 56.3965 |  86.72 KB |
| TestAddsNew |  25.60 us | 0.2226 us | 0.1859 us |  6.8359 |  10.55 KB |
