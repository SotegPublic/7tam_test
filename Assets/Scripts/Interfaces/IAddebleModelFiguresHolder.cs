public interface IAddebleModelFiguresHolder
{
    public void AddModel(FiguresTypes type, FigureView view);
    public FiguresCollectionModel GetCollectionModelByType(FiguresTypes type);
}