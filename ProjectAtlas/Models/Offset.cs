namespace ProjectAtlas.Models
{
    public class Offset
    {
        public string Name { get; set; }

        public int[] BaseChain { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }

        public int DirX { get; set; }
        public int DirY { get; set; }
        public int DirZ { get; set; }
    }
}