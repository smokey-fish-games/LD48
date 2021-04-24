using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    [SerializeField] private Neighbour[] neighbours;
    public static Node[] allNodes = null;
    [SerializeField] private float floatToIntFactor = 10f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        foreach (Neighbour n in neighbours)
        {
            n.distance = calculateNodeDistance(n.neighbour);
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (allNodes == null)
        {
            allNodes = FindObjectsOfType<Node>();
        }
    }

    int calculateNodeDistance(Node destination)
    {
        Vector2 start = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 end = new Vector2(destination.transform.position.x, destination.transform.position.y);

        // Calculate distance to node 
        float distF = Vector2.Distance(start, end);
        float preInt = distF * floatToIntFactor;
        return Mathf.RoundToInt(preInt);
    }

    void OnDrawGizmosSelected()
    {
        foreach (Neighbour n in neighbours)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, n.neighbour.transform.position);
        }
    }

    public Neighbour[] GetNeighbours()
    {
        return neighbours;
    }

    public static List<Node> findPath(Node start, Node destination)
    {
        if (start == null)
        {
            return null;
        }
        if (destination == null)
        {
            return null;
        }

        PathCalculator p = new PathCalculator();
        if (allNodes == null)
        {
            allNodes = FindObjectsOfType<Node>();
        }
        return p.findPath(allNodes, start, destination);
    }
}

[System.Serializable]
public class Neighbour
{
    public Node neighbour;
    public int distance;
}

class PathCalculator
{
    Dictionary<Node, int> distMap;
    Dictionary<Node, bool> visitedSet;
    List<Node> reverseRoute;
    bool foundEnd;

    public List<Node> findPath(Node[] allNodes, Node start, Node destination)
    {
        Node currentNode = start;

        visitedSet = new Dictionary<Node, bool>();
        distMap = new Dictionary<Node, int>();
        foundEnd = false;
        reverseRoute = new List<Node>();

        if (start == destination)
        {
            reverseRoute.Add(start);
            return reverseRoute;
        }

        foreach (Node n in allNodes)
        {
            int startCost = 0;
            if (n == start)
            {
                startCost = 0;
            }
            else
            {
                startCost = int.MaxValue;
            }

            distMap.Add(n, startCost);
            visitedSet.Add(n, false);
        }

        while (!foundEnd)
        {
            updateNeighbourCosts(currentNode);
            if (distMap[destination] != int.MaxValue)
            {
                // Found the end
                foundEnd = true;
            }
            else
            {
                // find next node to visit
                int smallest = int.MaxValue;
                foreach (Node n in allNodes)
                {
                    if (visitedSet[n])
                    {
                        continue;
                    }
                    if (distMap[n] < smallest)
                    {
                        smallest = distMap[n];
                        currentNode = n;
                    }
                }
            }
        }

        // build the route
        reverseRoute.Add(destination);
        currentNode = destination;
        while (currentNode != start)
        {
            Node n = findBackRoute(currentNode);
            reverseRoute.Add(n);
            currentNode = n;
            string route = "";
            foreach (Node r in reverseRoute)
            {
                route += r.name + "=>";
            }
        }

        // Flip the route list
        reverseRoute.Reverse();

        return reverseRoute;
    }

    Node findBackRoute(Node currentNode)
    {
        int myCost = distMap[currentNode];

        foreach (Neighbour n in currentNode.GetNeighbours())
        {
            if (visitedSet[n.neighbour])
            {
                int expectedCost = myCost - n.distance;

                if (expectedCost.Equals(distMap[n.neighbour]))
                {
                    return n.neighbour;
                }
            }
        }
        return null;
    }

    void updateNeighbourCosts(Node point)
    {
        int mycost = distMap[point];
        foreach (Neighbour n in point.GetNeighbours())
        {
            if (!visitedSet[n.neighbour])
            {
                int costCalc = mycost + n.distance;
                if (costCalc < distMap[n.neighbour])
                {
                    // new smaller cost
                    distMap[n.neighbour] = costCalc;
                }
            }
        }

        // Mark myself as visited
        visitedSet[point] = true;
    }
}