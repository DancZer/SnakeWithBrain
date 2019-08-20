using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeWithBrain
{
    interface IControl
    {
        void MoveLeft();
        void MoveRight();
        void MoveUp();
        void MoveDown();
    }
}
