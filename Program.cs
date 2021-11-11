using ASCIIDraw;

namespace C_Drawing{
    class Program{
       public static void Main(string[] Args){
            Screen playground = new Screen(60,42,new int[]{255,255,255});
            
            int[] R = new int[]{255,255,255,0,0,255};
            int[] G = new int[]{0,128,255,255,0,0};
            int[] B = new int[]{0,0,0,0,255,255};
            
            /*  
            for(int i=0;i<6;i++){  //pride flag - demo 2
                playground.Queue.Add(new Rectangle(new int[]{0,i%6*7},new int[]{120,42-(i%6*7)},new int[]{R[i%6],G[i%6],B[i%6]}));
            }
            
            for(int i=0;i<36;i++){ // pride rings - demo 1
                playground.Queue.Add(new Ring(new int[]{(i*3)-6,(i%6)*2},10,new int[]{R[i%6],G[i%6],B[i%6]},4));
            }
            */
            while(true==true){
                for(int i=1;i<30;i++){
                Thread.Sleep(1);
                playground.Queue.Add(new Ring(new int[]{0,0},i,new int[]{180,0,180},i));
                playground.Render();
                playground.Queue.Clear();
                }
            }
        }
    }
}