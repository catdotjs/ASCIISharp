using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ASCIIDraw{
    /* Structs */
    public struct Color{
        public int R,G,B;
        private int HexVal;
        public Color(int Hex){
            HexVal = Hex;
            R = Hex>>16;
            G = (Hex>>8) - (R<<8);
            B = Hex - ((Hex>>8)<<8);
        }
        public Color(int Red,int Green,int Blue){
            HexVal = (Red<<16)+(Green<<8)+Blue;
            R = Red;
            G = Green;
            B = Blue;
        }
        public override string ToString() => $"#{HexVal.ToString("X")}"; 
        public static implicit operator Color(int Hex) => new Color(Hex);
    }

    /* Abstract Classes*/
    abstract class Shape{
        public int x,y; //position x and y
        public Color ShapeColor = new Color(0xFFFFFF); //Shape Color
        public abstract bool RenderCheck(int xx,int yy);
    }

    /* new Scene render*/
    class Scene{
        public Scene(int Width,int Height, Color Foreground){
            {//NEVER LOOK HERE DW ABOUT THIS 
            var handle = kernel32.GetStdHandle(-11);int mode;kernel32.GetConsoleMode( handle, out mode );kernel32.SetConsoleMode( handle, mode | 0x4 );Console.CursorVisible=false;}
            ScreenWidth = Width;
            ScreenHeight = Height;
            DefaultForeground = Foreground;
        }
        public int ScreenWidth{get;}
        public int ScreenHeight{get;}
        public Color DefaultForeground = new Color(0xFFFFFF);
        public List<Shape> Queue = new List<Shape>();
        private string CurrentFrame = "";

        public void RenderFrame(object DebugInfo){
            Console.SetCursorPosition(0,0);
            Color CurrentColor; 

            //The big loop
            for(int i=0;i<(ScreenHeight*ScreenWidth);i++){
                // Coords
                int x = i%ScreenWidth;
                int y = i/ScreenWidth;
                //End  Line
                CurrentFrame+=(x==0)&(i!=0)?"\n":"";
                //Check Shapes
                try{CurrentColor = Queue.FirstOrDefault(shp => shp.RenderCheck(x,y)).ShapeColor;
                }catch{CurrentColor = DefaultForeground;}
                LoadToLine("██",CurrentColor);
            }
            Console.Write(CurrentFrame+"\n"+DebugInfo);
            CurrentFrame="";
        }
        private void LoadToLine(string Value,Color Color){
            CurrentFrame+=$"\x1b[38;2;{Color.R};{Color.G};{Color.B}m{Value}";
        }
    }

    /* Shapes and such*/
    class Ring : Shape{
        public Ring(int[] xy,int Radius,Color Color,int OutlineSize=1){
            x = xy[0];
            y = xy[1];
            r = Radius;
            o = OutlineSize;
            ShapeColor = Color;
        }
        public int r, o; //radius and outline size
        public override bool RenderCheck(int xx,int yy){
            int result = (int)(Math.Pow((xx-x)-r,2)+Math.Pow((yy-y)-r,2));
            return r<result==r*o>result;
        }
    }

    class Circle : Shape{
        public Circle(int[] xy,int Radius,Color Color){
            x = xy[0];
            y = xy[1];
            r = Radius;
            ShapeColor = Color;
        }
        public int r;//radius
        public override bool RenderCheck(int xx,int yy){
            int result = (int)(Math.Pow(xx-x,2)+Math.Pow(yy-y,2));
            return result<=Math.Pow(r,2);
        }
    }

    class Rectangle : Shape{
        public Rectangle(int[] xy,int[] wh,Color Color,bool isOutline=false){
            x = xy[0];
            y = xy[1];
            Width = wh[0];
            Height = wh[1];
            ShapeColor = Color;
            Outline = isOutline;
        }
        public int Width,Height;
        public bool Outline;
        public override bool RenderCheck(int xx,int yy){
            if(!Outline){
                return (xx>=x&&xx<=x+Width)&&(yy>=y&&yy<=y+Height); // bigger than x but smaller than x+width OR bigger than y but smaller than y+height
            }else{
                return ((xx>=x&&xx<=x+Width)&&(yy==y|yy==y+Height))|((yy>=y&&yy<=y+Height)&&(xx==x|xx==x+Width)); //idk how this works
            }
        }
    }

    class Point : Shape{
        public Point(int[] xy,Color Color){
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
    }
}