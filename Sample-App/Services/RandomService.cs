using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Services
{
    public class RandomService : IRandomService
    {
        private int _randomNumber;

        public RandomService()
        {
            Random random = new Random();
            _randomNumber = random.Next(10000);
        }
        public int GetNumber()
        {
            return _randomNumber;
        }
    }
}
