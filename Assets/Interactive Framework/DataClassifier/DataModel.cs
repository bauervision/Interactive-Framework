

namespace InteractiveFramework.DataClassifier
{

    #region Core Data Related
    [System.Serializable]
    public class ClassifierData
    {
        public string lastUpdate;
        public Scenario[] scenarios;

        public ClassifierData()
        {
            // last update was right now
            this.lastUpdate = System.DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            this.scenarios = new Scenario[] { };
        }

    }


    [System.Serializable]
    public class Scenario
    {
        public string lastUpdate;
        public double latitude;
        public double longitude;
        public ScenarioActor[] assets;
        public int id;
        public string name;
        public int siteIndex;

        public Scenario(string newName, int siteIndex)
        {
            this.lastUpdate = System.DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            this.assets = new ScenarioActor[] { };
            this.name = newName;
            System.Random rndID = new System.Random();
            this.id = rndID.Next(1000000, 9999999);
            this.siteIndex = siteIndex;

        }


    }


    [System.Serializable]
    public class ActorPosition
    {
        public double positionX;
        public double positionY;
        public double positionZ;

        public double rotationX;
        public double rotationY;
        public double rotationZ;

        public ActorPosition() { }

        public ActorPosition(ActorPosition data)
        {
            this.positionX = data.positionX;
            this.positionY = data.positionY;
            this.positionZ = data.positionZ;
            this.rotationX = data.rotationX;
            this.rotationY = data.rotationY;
            this.rotationZ = data.rotationZ;
        }
    }

    [System.Serializable]
    public class AssetModel
    {
        public string name;
        public float scale;

    }

    [System.Serializable]
    public class ScenarioActor
    {
        public int id;
        public string name;
        public string prefabName;
        public ActorPosition position;

        public ScenarioActor(string newName)
        {
            System.Random aID = new System.Random();
            this.id = aID.Next(1000000, 9999999);

            this.name = newName;
            this.position = new ActorPosition();
        }

        public ScenarioActor(ScenarioActor actor)
        {
            System.Random aID = new System.Random();
            this.id = aID.Next(1000000, 9999999);

            this.name = actor.name;
            this.position = new ActorPosition(actor.position);
        }


    }

    #endregion




}
