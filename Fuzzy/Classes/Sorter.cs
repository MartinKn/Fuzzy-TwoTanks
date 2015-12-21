using System;
using System.Collections;
using System.Windows.Forms;

public class Sorter : IComparer
{
    private CaseInsensitiveComparer ObjectCompare;      // Case insensitive comparer object
    private int ColumnToSort;                           // Specifies the column to be sorted
    private SortOrder OrderOfSort;                      // Specifies the order in which to sort (i.e. 'Ascending').

    public int SortColumn{
        set
        { ColumnToSort = value; }
        get
        { return ColumnToSort; }}

    public SortOrder Order{
        set
        { OrderOfSort = value; }
        get
        { return OrderOfSort; }}

    public Sorter()                       // Class constructor.  Initializes various elements
    {
        ColumnToSort = 0;                               // Initialize the column to '0'
        OrderOfSort = SortOrder.None;                   // Initialize the sort order to 'none'
        ObjectCompare = new CaseInsensitiveComparer();  // Initialize the CaseInsensitiveComparer object
    }

    /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
    /// <param name="x">First object to be compared</param>
    /// <param name="y">Second object to be compared</param>
    /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
    public int Compare(object x, object y)
    {
        try
        {
            int result=0;            
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;
            string s = listviewX.SubItems[0].ToString();
            s = listviewX.SubItems[1].ToString();
            s = listviewX.SubItems[2].ToString();
            s = listviewX.SubItems[3].ToString();


            switch(ColumnToSort)
            {
                case 0: result = compareN(listviewX, listviewY); break;
                default: result = compareS(listviewX, listviewY); break;
            }
            return result;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    private int compareS(ListViewItem listviewX, ListViewItem listviewY)
    {
        int compareResult;
        // Compare the two items
        compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
        if (OrderOfSort == SortOrder.Ascending)         // Calculate correct return value based on object comparison
        {
            return compareResult;                       // Ascending sort is selected, return normal result of compare operation
        }
        else
            if (OrderOfSort == SortOrder.Descending)
            {
                return (-compareResult);                    // Descending sort is selected, return negative result of compare operation
            }
            else
            {
                return 0;                                   // Return '0' to indicate they are equal
            }
    }

    private int compareN(ListViewItem listviewX, ListViewItem listviewY)
    {
        int compareResult;
        compareResult = ObjectCompare.Compare(Convert.ToInt32(listviewX.SubItems[ColumnToSort].Text), Convert.ToInt32(listviewY.SubItems[ColumnToSort].Text));
        if (OrderOfSort == SortOrder.Ascending)         // Calculate correct return value based on object comparison
        {
            return compareResult;                       // Ascending sort is selected, return normal result of compare operation
        }
        else
            if (OrderOfSort == SortOrder.Descending)
            {
                return (-compareResult);                    // Descending sort is selected, return negative result of compare operation
            }
            else
            {
                return 0;                                   // Return '0' to indicate they are equal
            }
    }
}