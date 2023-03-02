/*
작성자: 최재호(cjh0798@gmail.com)
기능: Entity를 보관하고 가져오는 함수 제공
 */
using System.Collections.Generic;
using System.Linq;

public class EntityDatabase : Singleton<EntityDatabase>
{
    public Dictionary<int, Entity> EntityDic { get; private set; }

    public void Setup()
    {
        EntityDic = new Dictionary<int, Entity>();
    }

    // Entity 추가
    public void AddEntity(Entity entity)
    {
        if (EntityDic == null)
            EntityDic = new Dictionary<int, Entity>();

        EntityDic.Add(entity.ID, entity);
    }

    // Entity 가져오기
    public Entity GetEntity(int entityID)
    {
        Entity entity = EntityDic.First(x => x.Key == entityID).Value;
        if (entity != null)
        {
            return entity;
        }

        else
        {
            return null;
        }
    }

    // 모든 Entity 가져오기
    public List<Entity> GetAllEntities()
    {
        return EntityDic.Values.ToList();
    }

    // Entity 삭제
    public void RemoveEntity(Entity obj)
    {
        EntityDic.Remove(obj.ID);
    }

    // 모든 Entity 삭제
    public void RemoveAllEntities()
    {
        EntityDic.Clear();
    }


}
