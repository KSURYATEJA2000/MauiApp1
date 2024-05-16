using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class AppState
    {
        public int? cartCount { get; set; }
        public void setCartCount(int count)
        {
            cartCount = count;
            NotifyStateChanged();
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
