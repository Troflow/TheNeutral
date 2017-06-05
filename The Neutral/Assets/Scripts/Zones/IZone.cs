using UnityEngine;


namespace Neutral
{
    public interface IZone
    {
        //add the entity to the zone
        void add(GameObject entity);

        //spawn an entity into the zone
        void spawn(EnemyType enemyType, Quaternion rotation);

        //remove an entity from the zone
        void delete(GameObject entity);

        //check if entity is in the zone
        bool contains(GameObject entity);

        //set the entities zone name to the current zone
        void setEntityZone(GameObject entity);

        //get current count of entities in zone
        int entitiesInZone();
    }
}