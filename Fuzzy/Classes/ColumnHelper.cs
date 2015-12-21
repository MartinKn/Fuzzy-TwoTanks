using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

class ColumnHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HDITEM
    {
        public Int32 mask;
        public Int32 cxy;
        [MarshalAs(UnmanagedType.LPTStr)]
        public String pszText;
        public IntPtr hbm;
        public Int32 cchTextMax;
        public Int32 fmt;
        public Int32 lParam;
        public Int32 iImage;
        public Int32 iOrder;
    };

    private const Int32 HDI_FORMAT = 0x0004;
    private const Int32 HDF_LEFT = 0x0000;
    private const Int32 HDF_STRING = 0x4000;
    private const Int32 HDF_SORTUP = 0x0400;
    private const Int32 HDF_SORTDOWN = 0x0200;
    private const Int32 LVM_GETHEADER = 0x1000 + 31;  // LVM_FIRST + 31
    private const Int32 HDM_GETITEM = 0x1200 + 11;  // HDM_FIRST + 11
    private const Int32 HDM_SETITEM = 0x1200 + 12;  // HDM_FIRST + 12

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "SendMessage")]
    private static extern IntPtr SendMessageITEM(IntPtr Handle, Int32 msg, IntPtr wParam, ref HDITEM lParam);

    public static void SetColumnHeaderSortIcon(ListView listView, int colIndex, SortOrder order)
    {
        IntPtr hColHeader = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
        HDITEM hdItem = new HDITEM();
        IntPtr colHeader = new IntPtr(colIndex);
        hdItem.mask = HDI_FORMAT;
        IntPtr rtn = SendMessageITEM(hColHeader, HDM_GETITEM, colHeader, ref hdItem);
        if (order == SortOrder.Ascending)
        {
            hdItem.fmt &= ~HDF_SORTDOWN;
            hdItem.fmt |= HDF_SORTUP;
        }
        else if (order == SortOrder.Descending)
        {
            hdItem.fmt &= ~HDF_SORTUP;
            hdItem.fmt |= HDF_SORTDOWN;
        }
        else if (order == SortOrder.None)
        {
            hdItem.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP;
        }
        rtn = SendMessageITEM(hColHeader, HDM_SETITEM, colHeader, ref hdItem);
    }
}
