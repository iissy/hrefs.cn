namespace ASY.Hrefs.Util.UIHelpers
{
    public abstract class MemoryStructure
    {
        public abstract int this[int i, int j] { get; set; }
    }

    /// <summary>
    /// Use this unless you get out of memory exceptions when comparing large strings
    /// </summary>
    public class RectangularArray : MemoryStructure
    {
        private int[,] d;

        public RectangularArray(int m, int n)
        {
            d = new int[m, n];
        }

        public override int this[int i, int j]
        {
            get { return d[i, j]; }
            set { d[i, j] = value; }
        }
    }

    /// <summary>
    /// If the product of the lengths of the strings is greater than 536,848,900 you need to use this structure to avoid the 2GB CLR limit on single objects
    /// </summary>
    public class JaggedArray : MemoryStructure
    {
        private int[][] d;


        public JaggedArray(int m, int n)
        {
            d = new int[m][];

            for (int i = 0; i < m; i++)
                d[i] = new int[n];
        }

        public override int this[int i, int j]
        {
            get { return d[i][j]; }
            set { d[i][j] = value; }
        }
    }
}