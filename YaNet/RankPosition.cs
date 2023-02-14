namespace YaNet
{
    public class RankPosition
    {
        private int[] _rankLengths;
        private int[] _currentPosition;
        private int _currentRank;

        public int Length => _rankLengths.Length;
        public int Last => _currentPosition[_currentRank];

        public RankPosition(params int[] rankLengths)
        {
            _rankLengths = new int[rankLengths.Length];

            for (int i = 0; i < _rankLengths.Length; i++)
            {
                _rankLengths[i] = rankLengths[i];
            }

            _currentPosition = new int[_rankLengths.Length];

            _currentRank = _rankLengths.Length - 1;
        }

        public int this[int index] => _currentPosition[index];

        public bool MoveNext()
        {
            if (_currentPosition[_currentRank] + 1 >= _rankLengths[_currentRank])
            {
                if (_currentRank - 1 < 0)
                {
                    return false;
                }

                _currentPosition[_currentRank] = 0;

                _currentRank--;

                return MoveNext();
            }

            _currentPosition[_currentRank]++;

            _currentRank = _rankLengths.Length - 1;

            return true;
        }

        public void Reset()
        {
            _currentPosition = new int[_rankLengths.Length];
        }

        public static int[] GetMaxRanks(Array array)
        {
            int[] maxRanks = new int[array.Rank];

            for (int i = 0; i < array.Rank; i++)
            {
                maxRanks[i] = array.GetLength(i);
            }

            return maxRanks;
        }

        public static implicit operator RankPosition(int[] rank) => new RankPosition(rank);

        public static implicit operator int[](RankPosition rankPosition) => rankPosition._currentPosition;

        public static implicit operator String(RankPosition rankPosition) => rankPosition.ToString();

        public override string ToString() => $"[{String.Join(", ", _currentPosition)}]";

        public static bool operator ==(RankPosition left, RankPosition right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(RankPosition left, RankPosition right) => !(left == right);

        public override bool Equals(object obj) => base.Equals(obj);
        
        public override int GetHashCode() => ToString().GetHashCode();
    }
}