using UnityEngine;


namespace Neutral
{
    public interface IZone
    {
        void add(GameObject entity);
        void spawn(EnemyType enemyType, Quaternion rotation);
        void delete(GameObject entity);
        bool contains(GameObject entity);
        void setEntityZone(GameObject entity);
        int entitiesInZone();
    }
}