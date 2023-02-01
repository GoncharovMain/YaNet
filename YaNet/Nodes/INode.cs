namespace YaNet.Nodes
{
    public interface INode
    {
        void Init(ref object obj, StringBuilder buffer);
        void Print(StringBuilder buffer);
    }
}