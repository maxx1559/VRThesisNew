using System;
using System.Runtime.InteropServices;

namespace Assets.GEL
{
    // Interface to GELs HMesh
    public abstract class IManifold
    {

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern IntPtr Manifold_new();

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern IntPtr Manifold_copy(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void Manifold_delete(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_positions(IntPtr manifold, double[,] pos);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void Manifold_pos(IntPtr manifold, int vertex_id, double[] pos);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_no_allocated_vertices(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_no_allocated_faces(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_no_allocated_halfedges(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_no_vertices(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_no_faces(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_no_halfedges(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_no_polygon_indices(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_halfedges(IntPtr manifold, IntPtr[] edges);

        [DllImport("Assets/Plugins/GELExt.dll")]
        // verts -> IntVector
        protected static extern int Manifold_vertices(IntPtr manifold, IntPtr[] verts);

        [DllImport("Assets/Plugins/GELExt.dll")]
        // faces -> IntVector
        protected static extern int Manifold_faces(IntPtr manifold, IntPtr[] faces);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void Manifold_add_face(IntPtr self, int no_verts, double[] pos);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void Manifold_remove_face(IntPtr self, int fid);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void randomize_mesh(IntPtr manifold, int max_iter);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void extrude_faces(IntPtr manifold, int face_number, int[] faces);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void triangulate(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void to_idfs(IntPtr manifold, double[] points, int[] triangles);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void get_hmesh_ids(IntPtr m, int[] vertex_ids, int[] halfedge_ids, int[] face_ids);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern bool Manifold_vertex_in_use(IntPtr manifold, int vertex_id);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern bool Manifold_face_in_use(IntPtr manifold, int face_id);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern bool Manifold_halfedge_in_use(IntPtr manifold, int halfedge_id);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void move_vertex_along_vector(IntPtr manifold, int vertex_id, double[] direction);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void move_face_along_vector(IntPtr manifold, int face_id, double[] direction);
        
        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void rotate_faces_around_point(IntPtr manifold, int number_of_faces, int[] face_ids,
            double[] point, double[] rotation);
       
        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void move_faces_along_vector(IntPtr manifold, int number_of_faces, int[] faceIds, double[] direction);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void move_faces_along_normal(IntPtr manifold, int number_of_faces, int[] faceIds, double magnitude);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void stitch_mesh(IntPtr manifold, double radius);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void Manifold_cleanup(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void log_manifold(IntPtr manifold, char[] file_name);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void center(IntPtr manifold, int face_id, double[] center);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void center_triangulated(IntPtr manifold, int face_id, double[] center);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void get_neighbouring_faces_and_edge(IntPtr manifold, int face_id, int[] neighbour_faces, double[] edge_centers, double[] edge_direction, int[] edge_ids);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int no_edges(IntPtr manifold, int face_id);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void face_normal(IntPtr manifold, int face_id, double[] normal);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void vertex_normal(IntPtr manifold, int vertex_id, double[] normal);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void test_extrude_and_move(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void get_first_edge_direction(IntPtr manifold, int face_id, double[] normal);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void log_manifold_original(IntPtr manifold, char[] file_name);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int collapse_short_edges(IntPtr manifold, double threshold, int[] movingVertices, int movingVerticesCount);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern void test_valery(IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Walker_opposite_halfedge(IntPtr manifold, int halfedgeId);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Walker_incident_vertex(IntPtr manifold, int halfedgeId);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern bool obj_load(string file_name, IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern bool x3d_load(string file_name, IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern bool obj_save(string file_name, IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern bool x3d_save(string file_name, IntPtr manifold);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_split_face_by_edge(IntPtr manifold, int face, int edge1, int edge2);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_split_edge(IntPtr manifold, int h);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Walker_next_halfedge(IntPtr manifold, int halfedgeId);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Walker_incident_face(IntPtr manifold, int halfedgeId);

        [DllImport("Assets/Plugins/GELExt.dll")]
        protected static extern int Manifold_bridge_faces(IntPtr manifold, int face1, int face2, int[] f1vids, int[] f2vids, int noVertPairs);
        
    }
}