using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace desktop
{
    public class ListBoxItemCard : ListBoxItem
    {
        private int Index;
        private bool IsIndexSet = false;

        public int GetIndex()
        {
            return Index;
        }

        public void SetIndex(int index)
        {
            if(!IsIndexSet) 
            {
                Index = index;
                IsIndexSet = true;
            }            
        }
    }
}
