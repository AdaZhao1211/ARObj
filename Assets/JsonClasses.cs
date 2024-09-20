// JsonClasses.cs

using System.Collections.Generic;

public class Category
{
    public float score { get; set; }
    public int index { get; set; }
    public string categoryName { get; set; }
    public string displayName { get; set; }
}

public class BoundingBox
{
    public float originX { get; set; }
    public float originY { get; set; }
    public float width { get; set; }
    public float height { get; set; }
    public float angle { get; set; }
}

public class Detection
{
    public List<Category> categories { get; set; }
    public List<object> keypoints { get; set; }
    public BoundingBox boundingBox { get; set; }
}

public class RootObject
{
    public List<int> videoInfo { get; set; }
    public List<Detection> detections { get; set; }
}
