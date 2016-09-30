using Android.Runtime;
using System;

namespace Mathematician.Vision.JavaBinding
{
    public static class JavaBindingHelper
    {
        [Register("WARP_INVERSE_MAP")]
        public const int WarpInverseMap = (int)16;
        internal static IntPtr java_class_handle;
        internal static IntPtr class_ref
        {
            get
            {
                return JNIEnv.FindClass("org/opencv/imgproc/Imgproc", ref java_class_handle);
            }
        }

        static IntPtr id_findContours_Lorg_opencv_core_Mat_Ljava_util_List_Lorg_opencv_core_Mat_II;
        // Metadata.xml XPath method reference: path="/api/package[@name='org.opencv.imgproc']/class[@name='Imgproc']/method[@name='findContours' and count(parameter)=5 and parameter[1][@type='org.opencv.core.Mat'] and parameter[2][@type='java.util.List&lt;org.opencv.core.MatOfPoint&gt;'] and parameter[3][@type='org.opencv.core.Mat'] and parameter[4][@type='int'] and parameter[5][@type='int']]"
        [Register("findContours", "(Lorg/opencv/core/Mat;Ljava/util/List;Lorg/opencv/core/Mat;II)V", "")]
        public static unsafe void Imgproc_FindContours(global::OpenCV.Core.Mat p0, Java.Util.ArrayList p1, global::OpenCV.Core.Mat p2, int p3, int p4)
        {
            if (id_findContours_Lorg_opencv_core_Mat_Ljava_util_List_Lorg_opencv_core_Mat_II == IntPtr.Zero)
                id_findContours_Lorg_opencv_core_Mat_Ljava_util_List_Lorg_opencv_core_Mat_II = JNIEnv.GetStaticMethodID(class_ref, "findContours", "(Lorg/opencv/core/Mat;Ljava/util/List;Lorg/opencv/core/Mat;II)V");
            //IntPtr native_p1 = global::Android.Runtime.JavaList<global::OpenCV.Core.MatOfPoint>.ToLocalJniHandle(p1);
            JValue* __args = stackalloc JValue[5];
            __args[0] = new JValue(p0);
            __args[1] = new JValue(p1);
            __args[2] = new JValue(p2);
            __args[3] = new JValue(p3);
            __args[4] = new JValue(p4);
            JNIEnv.CallStaticVoidMethod(class_ref, id_findContours_Lorg_opencv_core_Mat_Ljava_util_List_Lorg_opencv_core_Mat_II, __args);
        }
    }
}