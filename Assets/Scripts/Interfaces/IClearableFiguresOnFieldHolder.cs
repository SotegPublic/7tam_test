using System.Collections.Generic;

public interface IClearableFiguresOnFieldHolder
{
    List<FiguresCollectionModel> GetCollectionsForClear();
    public void RemoveFiguresCollection(FiguresCollectionModel figuresCollectionModel);
}
