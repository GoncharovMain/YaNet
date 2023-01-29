namespace YaNet.Nodes
{
    public interface INode
    {
        void Init(object obj, StringBuilder buffer);
        void Print(StringBuilder buffer);
    }
}