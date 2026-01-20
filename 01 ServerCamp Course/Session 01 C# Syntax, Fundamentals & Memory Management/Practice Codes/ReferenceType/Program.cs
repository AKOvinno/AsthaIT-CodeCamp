// We have to enable 'allowUnsafeBlocks' in the .csproj project file to use unsafe code.
unsafe
{
    int num = 42;
    int* AddressOfNum = &num;
    System.Console.WriteLine($"Value: {*AddressOfNum}, Address: {(ulong)AddressOfNum}");
}

