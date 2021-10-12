namespace Simple.Dotnet.Cloning.Cloners
{
    internal static class MultiDimArrayCloner
    {
        public static class TwoDim
        {
            public static T[,] ShallowClone<T>(T[,] array)
            {
                if (array == null) return null;

                var (firstDimLen, secondDimLen) = (array.GetLength(0), array.GetLength(1));
                if (array.Length == 0) return new T[firstDimLen, secondDimLen];

                var clone = new T[firstDimLen, secondDimLen];
                for (var i = 0; i < firstDimLen; i++)
                {
                    for (var j = 0; j < secondDimLen; j++) clone[i, j] = array[i, j];
                }

                return clone;
            }

            public static T[,] DeepClone<T>(T[,] array)
            {
                if (array == null) return null;

                var (firstDimLen, secondDimLen) = (array.GetLength(0), array.GetLength(1));
                if (array.Length == 0) return new T[firstDimLen, secondDimLen];

                var clone = new T[firstDimLen, secondDimLen];
                for (var i = 0; i < firstDimLen; i++)
                {
                    for (var j = 0; j < secondDimLen; j++) clone[i, j] = RootCloner<T>.DeepClone(array[i, j]);
                }

                return clone;
            }
        }

        public static class ThreeDim
        {
            public static T[,,] ShallowClone<T>(T[,,] array)
            {
                if (array == null) return null;

                var (firstDimLen, secondDimLen, thirdDimLen) = (array.GetLength(0), array.GetLength(1), array.GetLength(2));
                if (array.Length == 0) return new T[firstDimLen, secondDimLen, thirdDimLen];

                var clone = new T[firstDimLen, secondDimLen, thirdDimLen];
                for (var i = 0; i < firstDimLen; i++)
                {
                    for (var j = 0; j < secondDimLen; j++)
                    {
                        for (var k = 0; k < thirdDimLen; k++) clone[i, j, k] = array[i, j, k];
                    }
                }

                return clone;
            }

            public static T[,,] DeepClone<T>(T[,,] array)
            {
                if (array == null) return null;

                var (firstDimLen, secondDimLen, thirdDimLen) = (array.GetLength(0), array.GetLength(1), array.GetLength(2));
                if (array.Length == 0) return new T[firstDimLen, secondDimLen, thirdDimLen];

                var clone = new T[firstDimLen, secondDimLen, thirdDimLen];
                for (var i = 0; i < firstDimLen; i++)
                {
                    for (var j = 0; j < secondDimLen; j++)
                    {
                        for (var k = 0; k < thirdDimLen; k++) clone[i, j, k] = RootCloner<T>.DeepClone(array[i, j, k]);
                    }
                }

                return clone;
            }
        }

        public static class FourDim
        {

            public static T[,,,] ShallowClone<T>(T[,,,] array)
            {
                if (array == null) return null;

                var (firstDimLen, secondDimLen, thirdDimLen, forthDimLen) = (array.GetLength(0), array.GetLength(1), array.GetLength(2), array.GetLength(3));
                if (array.Length == 0) return new T[firstDimLen, secondDimLen, thirdDimLen, forthDimLen];

                var clone = new T[firstDimLen, secondDimLen, thirdDimLen, forthDimLen];
                for (var i = 0; i < firstDimLen; i++)
                {
                    for (var j = 0; j < secondDimLen; j++)
                    {
                        for (var k = 0; k < thirdDimLen; k++)
                        {
                            for (var t = 0; t < forthDimLen; t++) clone[i, j, k, t] = array[i, j, k, t];
                        }
                    }
                }

                return clone;
            }

            public static T[,,,] DeepClone<T>(T[,,,] array)
            {
                if (array == null) return null;

                var (firstDimLen, secondDimLen, thirdDimLen, forthDimLen) = (array.GetLength(0), array.GetLength(1), array.GetLength(2), array.GetLength(3));
                if (array.Length == 0) return new T[firstDimLen, secondDimLen, thirdDimLen, forthDimLen];

                var clone = new T[firstDimLen, secondDimLen, thirdDimLen, forthDimLen];
                for (var i = 0; i < firstDimLen; i++)
                {
                    for (var j = 0; j < secondDimLen; j++)
                    {
                        for (var k = 0; k < thirdDimLen; k++)
                        {
                            for (var t = 0; t < forthDimLen; t++) clone[i, j, k, t] = RootCloner<T>.DeepClone(array[i, j, k, t]);
                        }
                    }
                }

                return clone;
            }
        }
    }
}
