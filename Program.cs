using ASCIIDraw;
using System;
using System.Dynamic;

namespace C_Drawing{
    class Program{
       public static void Main(string[] Args){
            Scene playground = new Scene(78 ,42,new Color(160,36,86));
            
            
            // new Demo 2 - Bouncin Ball(s)
            Console.Clear();
            playground.Queue.Add(new Rectangle(new int[]{0,38},new int[]{playground.ScreenWidth,4},new Color(0xFFFFFF)));
            playground.Queue.Add(new Rectangle(new int[]{0,0},new int[]{playground.ScreenWidth,2},new Color(0xFFFFFF)));
            playground.Queue.Add(new Circle(new int[]{0,9},6,new Color(0xAF00F8)));
            int y2 = 0;
            while(true){
                int yy = playground.Queue[2].y;
                int xx = playground.Queue[2].x;
                y2++;
                playground.Queue[2].y+=(y2%48)<24?1:-1;
                playground.Queue[2].x++;
                playground.RenderFrame($"{xx},{yy}");
                if(xx>playground.ScreenWidth){
                playground.Queue[2].x=0;
                }
            }
            /**/
            /*
            //new Demo 1 - Prider Flag
            Color[] Colors = new Color[]{0xFF0000,0xFF8000,0xFFFF00,0x00FF00,0x0000FF,0xFF00FF};
            for(int i=0;i<6;i++){  
                playground.Queue.Add(new Rectangle(new int[]{0,i%6*7},new int[]{60,7},Colors[i]));
            }
            playground.RenderFrame();
            playground.Queue.Clear();
            */
        }
        /*  
        public static void oldTests(){
            int[] R = new int[]{255,255,255,0,0,255};
            int[] G = new int[]{0,128,255,255,0,0};
            int[] B = new int[]{0,0,0,0,255,255};
            for(int i=0;i<6;i++){  //pride flag - demo 2
                playground.Queue.Add(new Rectangle(new int[]{0,i%6*7},new int[]{120,42-(i%6*7)},new int[]{R[i%6],G[i%6],B[i%6]}));
            }
            playground.Render();
            
            for(int i=0;i<36;i++){ // pride rings - demo 1
                playground.Queue.Add(new Ring(new int[]{(i*3)-6,(i%6)*2},10,new int[]{R[i%6],G[i%6],B[i%6]},4));
            }
            while(true==true){ // animation - demo 3
                for(int i=1;i<60;i++){
                playground.Queue.Add(new Ring(new int[]{0,0},i,new int[]{180,0,180},i));
                playground.Render();
                Thread.Sleep(200);
                playground.Queue.Clear();
                }
            }
            
            // Circles - demo 4
            playground.Queue.Add(new Circle(new int[]{10,10},10,new int[]{255,0,255}));
            playground.Queue.Add(new Circle(new int[]{10,10},9,new int[]{255,255,0}));
            playground.Queue.Add(new Circle(new int[]{10,10},8,new int[]{0,255,0}));
            playground.Render();

            // Pixels! - demo 5
            Random rand = new Random();
            for(int i=0;i<60*42;i++){
                int r = rand.Next(1,255);
                int g = rand.Next(1,255);
                int b = rand.Next(1,255);
                playground.Queue.Add(new Point(new int[]{i%60,(i/60)},new int[]{r,g,b}));
            }
            playground.Render();
        }
        */
    }
}