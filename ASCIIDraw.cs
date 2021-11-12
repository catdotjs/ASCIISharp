using System;
using System.Runtime.InteropServices;

namespace ASCIIDraw{
    /* Abstract Classes*/
    abstract class Shape{
        public int x {get; set;} //position x
        public int y {get; set;} //position y
        public int[] ShapeColor = new int[3]; //Shape Color
        public abstract bool RenderCheck(int xx,int yy);
    }
    /* Screen */
    class Screen{
        public Screen(int Width,int Height, int[] Foreground){
            {//NEVER LOOK HERE DW ABOUT THIS 
                var handle = kernel32.GetStdHandle(-11);int mode;kernel32.GetConsoleMode( handle, out mode );kernel32.SetConsoleMode( handle, mode | 0x4 );}
            ScreenWidth = Width;
            ScreenHeight = Height;
            DefaultForeground = Foreground;
        }
        public int ScreenWidth{get;}
        public int ScreenHeight{get;}
        public int[] DefaultForeground = new int[3];
        public List<Shape> Queue = new List<Shape>();

        public void WriteColor(string Value,int[] Color){
            kernel32.OutputDebugString($"\x1b[38;2;{Color[0]};{Color[1]};{Color[2]}m{Value}");
        }
        public void Render(){
            /*
            There is a Render Queue that code checks here. 
            There is currently no Z-depth. last one to get calculate
            has highest Z-depth ¯\_(ツ)_/¯
            */
            
            Console.Clear();
            for(int y=0;y<ScreenHeight;y++){
            for(int x=0;x<ScreenWidth;x++){
                int[] result = DefaultForeground;
                foreach(Shape shp in Queue){
                   result = shp.RenderCheck(x,y)==true?shp.ShapeColor:result; //check render queue here and draw colour
                }
                WriteColor("█",result);
            }

            Console.WriteLine();
            }
        }
        public void ClearScene(){
            /* Clear screen */
         Console.Clear();
        for(int y=0;y<ScreenHeight;y++){
        for(int x=0;x<ScreenWidth;x++){
            WriteColor("#",DefaultForeground);
        }
        Console.WriteLine();
        }
    }
    }

    /* Shapes*/
    class Ring : Shape{
        public Ring(int[] xy,int Radius,int[] Color,int OutlineSize=1){
            x = xy[0];
            y = xy[1];
            r = Radius;
            o = OutlineSize;
            ShapeColor = Color;
        }
        public int r {get; set;} //radius
        public int o {get; set;} //outline size
        public override bool RenderCheck(int xx,int yy){
            int result = (int)(Math.Pow((xx-x)-r,2)+Math.Pow((yy-y)-r,2));
            return r<result==r*o>result;
        }
    }

    class Circle : Shape{
        public Circle(int[] xy,int Radius,int[] Color){
            x = xy[0];
            y = xy[1];
            r = Radius;
            ShapeColor = Color;
        }
        public int r {get; set;} //radius
        public override bool RenderCheck(int xx,int yy){
            int result = (int)(Math.Pow(xx-x,2)+Math.Pow(yy-y,2));
            return result<=Math.Pow(r,2);
        }
    }

    class Rectangle : Shape{
        public Rectangle(int[] xy,int[] wh,int[] Color,bool isOutline=false){
            x = xy[0];
            y = xy[1];
            Width = wh[0];
            Height = wh[1];
            ShapeColor = Color;
            Outline = isOutline;
        }
        public int Width{get; set;}
        public int Height{get; set;}
        public bool Outline{get; set;}
        public override bool RenderCheck(int xx,int yy){
            if(!Outline){
                return (xx>=x&&xx<=x+Width)&&(yy>=y&&yy<=y+Height); // bigger than x but smaller than x+width OR bigger than y but smaller than y+height
            }else{
                return ((xx>=x&&xx<=x+Width)&&(yy==y|yy==y+Height))|((yy>=y&&yy<=y+Height)&&(xx==x|xx==x+Width)); //idk how this works
            }
        }
    }

    class Point : Shape{
        public Point(int[] xy,int[] Color){
            x = xy[0];
            y = xy[1];
            ShapeColor = Color;
        }
        public override bool RenderCheck(int xx,int yy){
            return (xx==x&&yy==y);
        }
        }

    /* Colour stuff */
    static class kernel32{
        // https://newbedev.com/custom-text-color-in-c-console-application <> https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences?redirectedfrom=MSDN
       [DllImport( "kernel32.dll", SetLastError = true )]
       public static extern bool SetConsoleMode( IntPtr hConsoleHandle, int mode );
       [DllImport( "kernel32.dll", SetLastError = true )]
       public static extern bool GetConsoleMode( IntPtr handle, out int mode );
       [DllImport( "kernel32.dll", SetLastError = true )]
       public static extern IntPtr GetStdHandle( int handle );
       [DllImport("kernel32.dll")]
       public static extern void OutputDebugString(string lpOutputString);
    }
}