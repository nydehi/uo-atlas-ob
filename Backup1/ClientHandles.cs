using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Ultima;

namespace Ultima
{
	public class ClientWindowHandle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		public static ClientWindowHandle Invalid = new ClientWindowHandle( new IntPtr( -1 ) );

		public ClientWindowHandle()
		{
		}

		public ClientWindowHandle( IntPtr value )			
		{
			handle = value;
		}

		protected override bool ReleaseHandle()
		{
			return ReleaseHandle();
		}
	}

	public class ClientProcessHandle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		public static ClientProcessHandle Invalid = new ClientProcessHandle( new IntPtr( -1 ) );

		public ClientProcessHandle()
			: base()
		{			
		}

		public ClientProcessHandle( IntPtr value )
			: base()
		{
			handle = value;
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.CloseHandle(this) == 0;
		}
	}
}
