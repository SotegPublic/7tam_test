public interface IChangableIcyFiguresSystem
{
    public void AddIcyFigure(FigureView icyView);
    public void FullClear();
    public void ClearCurrentCollection();
    public int CurrentDeletedFigures { get; }
}
