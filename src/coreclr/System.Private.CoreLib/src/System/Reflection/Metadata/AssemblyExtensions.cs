// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection.Metadata
{
    public static partial class AssemblyExtensions
    {
        [GeneratedDllImport(RuntimeHelpers.QCall, EntryPoint = "AssemblyNative_InternalTryGetRawMetadata")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static unsafe partial bool InternalTryGetRawMetadata(QCallAssembly assembly, ref byte* blob, ref int length);

        // Retrieves the metadata section of the assembly, for use with System.Reflection.Metadata.MetadataReader.
        //   - Returns false upon failure. Metadata might not be available for some assemblies, such as AssemblyBuilder, .NET
        //     native images, etc.
        //   - Callers should not write to the metadata blob
        //   - The metadata blob pointer will remain valid as long as the AssemblyLoadContext with which the assembly is
        //     associated, is alive. The caller is responsible for keeping the assembly object alive while accessing the
        //     metadata blob.
        [CLSCompliant(false)] // out byte* blob
        public static unsafe bool TryGetRawMetadata(this Assembly assembly, out byte* blob, out int length)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            blob = null;
            length = 0;

            var runtimeAssembly = assembly as RuntimeAssembly;
            if (runtimeAssembly == null)
            {
                return false;
            }

            RuntimeAssembly rtAsm = runtimeAssembly;

            return InternalTryGetRawMetadata(new QCallAssembly(ref rtAsm), ref blob, ref length);
        }
    }
}
