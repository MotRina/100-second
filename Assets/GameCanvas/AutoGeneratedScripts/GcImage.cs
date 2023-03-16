/*------------------------------------------------------------*/
// <summary>GameCanvas for Unity</summary>
// <author>Seibe TAKAHASHI</author>
// <remarks>
// (c) 2015-2022 Smart Device Programming.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </remarks>
/*------------------------------------------------------------*/
#nullable enable

namespace GameCanvas
{
    public readonly partial struct GcImage : System.IEquatable<GcImage>
    {
        internal const int __Length__ = 7;
        public static readonly GcImage Alien = new GcImage("Alien", 2048, 2048);
        public static readonly GcImage BackGround = new GcImage("BackGround", 564, 1002);
        public static readonly GcImage Bullet = new GcImage("Bullet", 1216, 1216);
        public static readonly GcImage Cloud = new GcImage("Cloud", 2048, 2048);
        public static readonly GcImage LeftPlane = new GcImage("LeftPlane", 2000, 2000);
        public static readonly GcImage Pause = new GcImage("Pause", 1600, 1600);
        public static readonly GcImage RightPlane = new GcImage("RightPlane", 2000, 2000);
    }
}
