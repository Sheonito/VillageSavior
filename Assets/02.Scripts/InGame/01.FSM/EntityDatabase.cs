/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: Entity�� �����ϰ� �������� �Լ� ����
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

    // Entity �߰�
    public void AddEntity(Entity entity)
    {
        if (EntityDic == null)
            EntityDic = new Dictionary<int, Entity>();

        EntityDic.Add(entity.ID, entity);
    }

    // Entity ��������
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

    // ��� Entity ��������
    public List<Entity> GetAllEntities()
    {
        return EntityDic.Values.ToList();
    }

    // Entity ����
    public void RemoveEntity(Entity obj)
    {
        EntityDic.Remove(obj.ID);
    }

    // ��� Entity ����
    public void RemoveAllEntities()
    {
        EntityDic.Clear();
    }


}
