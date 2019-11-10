namespace ASY.Hrefs.Util.UIHelpers
{
    /// <summary>
    /// 数据结构
    /// </summary>
    public abstract class MemoryStructure
    {
        public abstract int this[int i, int j] { get; set; }
    }

    /// <summary>
    /// 二维数组
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
    /// 交错数组
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