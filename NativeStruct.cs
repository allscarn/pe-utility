﻿using System;
using System.Runtime.InteropServices;

namespace PEUtility
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDosHeader
    {
        public UInt16 Signature;
        public UInt16 LastPageSize;
        public UInt16 PagesInFile;
        public UInt16 Relocations;
        public UInt16 HeaderSizePar;
        public UInt16 MinAlloc;
        public UInt16 MaxAlloc;
        public UInt16 Ss;
        public UInt16 Sp;
        public UInt16 Checksum;
        public UInt16 Ip;
        public UInt16 Cs;
        public UInt16 LfaRelocationTable;
        public UInt16 OverlayNumber;
        public UInt32 Reserved1;
        public UInt16 OemId;
        public UInt16 OemInfo;
        public UInt64 Reserved2;
        public UInt64 Reserved3;
        public UInt32 Reserved4;
        public Int32 LfaNewHeader;

        public bool IsValid
        {
            get { return Signature == 0x5a4d; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageFileHeader
    {
        public UInt16 Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public UInt16 Characteristics;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDataDirectory
    {
        public UInt32 VirtualAddress;
        public UInt32 Size;
    }

    public enum Subsystem : ushort
    {
        Unknown = 0,
        Native = 1,
        WindowsGui = 2,
        WindowsCui = 3,
        PosixCui = 7,
        WindowsCeGui = 9,
        EfiApplication = 10,
        EfiBootServiceDriver = 11,
        EfiRuntimeDriver = 12,
        EfiRom = 13,
        Xbox = 14
    }

    [Flags]
    public enum DllCharacteristics : ushort
    {
        DynamicBase = 0x0040,
        ForceIntegrity = 0x0080,
        NxCompat = 0x0100,
        NoIsolation = 0x0200,
        NoSeh = 0x0400,
        NoBind = 0x0800,
        WdmDriver = 0x2000,
        TerminalServerAware = 0x8000
    }

    public enum ImageOptionalHeaderMagic : ushort
    {
        Header32 = 0x10b,
        Header64 = 0x20b,
        HeaderRom = 0x107
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageOptionalHeader32
    {
        public ImageOptionalHeaderMagic Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public UInt32 SizeOfCode;
        public UInt32 SizeOfInitializedData;
        public UInt32 SizeOfUninitializedData;
        public UInt32 AddressOfEntryPoint;
        public UInt32 BaseOfCode;
        public UInt32 BaseOfData;
        public UInt32 ImageBase;
        public UInt32 SectionAlignment;
        public UInt32 FileAlignment;
        public UInt16 MajorOperatingSystemVersion;
        public UInt16 MinorOperatingSystemVersion;
        public UInt16 MajorImageVersion;
        public UInt16 MinorImageVersion;
        public UInt16 MajorSubsystemVersion;
        public UInt16 MinorSubsystemVersion;
        public UInt32 Win32VersionValue;
        public UInt32 SizeOfImage;
        public UInt32 SizeOfHeaders;
        public UInt32 CheckSum;
        public Subsystem Subsystem;
        public DllCharacteristics DllCharacteristics;
        public UInt32 SizeOfStackReserve;
        public UInt32 SizeOfStackCommit;
        public UInt32 SizeOfHeapReserve;
        public UInt32 SizeOfHeapCommit;
        public UInt32 LoaderFlags;
        public UInt32 NumberOfRvaAndSizes;
        public ImageDataDirectory ExportTable;
        public ImageDataDirectory ImportTable;
        public ImageDataDirectory ResourceTable;
        public ImageDataDirectory ExceptionTable;
        public ImageDataDirectory CertificateTable;
        public ImageDataDirectory BaseRelocationTable;
        public ImageDataDirectory Debug;
        public ImageDataDirectory Architecture;
        public ImageDataDirectory GlobalPtr;
        public ImageDataDirectory TLSTable;
        public ImageDataDirectory LoadConfigTable;
        public ImageDataDirectory BoundImport;
        public ImageDataDirectory IAT;
        public ImageDataDirectory DelayImportDescriptor;
        public ImageDataDirectory CLRRuntimeHeader;
        public ImageDataDirectory Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageOptionalHeader64
    {
        public ImageOptionalHeaderMagic Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public UInt32 SizeOfCode;
        public UInt32 SizeOfInitializedData;
        public UInt32 SizeOfUninitializedData;
        public UInt32 AddressOfEntryPoint;
        public UInt32 BaseOfCode;
        public UInt64 ImageBase;
        public UInt32 SectionAlignment;
        public UInt32 FileAlignment;
        public UInt16 MajorOperatingSystemVersion;
        public UInt16 MinorOperatingSystemVersion;
        public UInt16 MajorImageVersion;
        public UInt16 MinorImageVersion;
        public UInt16 MajorSubsystemVersion;
        public UInt16 MinorSubsystemVersion;
        public UInt32 Win32VersionValue;
        public UInt32 SizeOfImage;
        public UInt32 SizeOfHeaders;
        public UInt32 CheckSum;
        public Subsystem Subsystem;
        public DllCharacteristics DllCharacteristics;
        public UInt64 SizeOfStackReserve;
        public UInt64 SizeOfStackCommit;
        public UInt64 SizeOfHeapReserve;
        public UInt64 SizeOfHeapCommit;
        public UInt32 LoaderFlags;
        public UInt32 NumberOfRvaAndSizes;
        public ImageDataDirectory ExportTable;
        public ImageDataDirectory ImportTable;
        public ImageDataDirectory ResourceTable;
        public ImageDataDirectory ExceptionTable;
        public ImageDataDirectory CertificateTable;
        public ImageDataDirectory BaseRelocationTable;
        public ImageDataDirectory Debug;
        public ImageDataDirectory Architecture;
        public ImageDataDirectory GlobalPtr;
        public ImageDataDirectory TLSTable;
        public ImageDataDirectory LoadConfigTable;
        public ImageDataDirectory BoundImport;
        public ImageDataDirectory IAT;
        public ImageDataDirectory DelayImportDescriptor;
        public ImageDataDirectory CLRRuntimeHeader;
        public ImageDataDirectory Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageNtHeaders32
    {
        public UInt32 Signature;
        public ImageFileHeader FileHeader;
        public ImageOptionalHeader32 OptionalHeader;

        public bool IsValid
        {
            get { return Signature == 0x4550 &&
                (OptionalHeader.Magic == ImageOptionalHeaderMagic.Header32 || OptionalHeader.Magic == ImageOptionalHeaderMagic.Header64);
            }
        }

        public bool Is64Bit
        {
            get { return OptionalHeader.Magic == ImageOptionalHeaderMagic.Header64; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageNtHeaders64
    {
        public UInt32 Signature;
        public ImageFileHeader FileHeader;
        public ImageOptionalHeader64 OptionalHeader;

        public bool IsValid
        {
            get
            {
                return Signature == 0x4550 &&
                    (OptionalHeader.Magic == ImageOptionalHeaderMagic.Header32 || OptionalHeader.Magic == ImageOptionalHeaderMagic.Header64);
            }
        }
    }

    public enum ImageDirectoryEntry
    {
        Export = 0,
        Import = 1,
        Resource = 2,
        Exception = 3,
        Security = 4,
        BaseReloc = 5,
        Debug = 6,
        Architecture = 7,
        GlobalPtr = 8,
        Tls = 9,
        LoadConfig = 10,
        BoundImport = 11,
        Iat = 12,
        DelayImport = 13,
        ComDescriptor = 14
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageSectionHeader
    {
        public UInt64 Name;
        public UInt32 VirtualSize;
        public UInt32 VirtualAddress;
        public UInt32 SizeOfRawData;
        public UInt32 PointerToRawData;
        public UInt32 PointerToRelocations;
        public UInt32 PointerToLinenumbers;
        public UInt16 NumberOfRelocations;
        public UInt16 NumberOfLinenumbers;
        public UInt32 Characteristics;

        public string Section
        {
            get
            {
                var name = new char[8];
                int i, length = 0;
                for (i = 0; i < 8; i++)
                {
                    name[i] = (char)(Name >> (i << 3) & 0xffUL);
                    if (name[i] == 0)
                        break;
                }
                length = i;
                return new string(name, 0, length);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ImageImportDescriptor
    {
        public UInt32 Characteristics;
        //public UInt32 OriginalFirstThunk;
        public UInt32 TimeDateStamp;
        public UInt32 ForwarderChain;
        public UInt32 Name;
        public UInt32 FirstThunk;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageExportDirectory
    {
        public UInt32 Characteristics;
        public UInt32 TimeDateStamp;
        public UInt16 MajorVersion;
        public UInt16 MinorVersion;
        public UInt32 Name;
        public UInt32 Base;
        public UInt32 NumberOfFunctions;
        public UInt32 NumberOfNames;
        public UInt32 AddressOfFunctions;
        public UInt32 AddressOfNames;
        public UInt32 AddressOfNameOrdinals;

        public string ExportDirectory
        {
            get
            {
                var name = new char[4];
                int i, length = 0;
                for (i = 0; i < 4; i++)
                {
                    name[i] = (char)(Name >> (i << 3) & 0xffUL);
                    if (name[i] == 0)
                        break;
                }
                length = i;
                return new string(name, 0, length);
            }
        }
    }
}