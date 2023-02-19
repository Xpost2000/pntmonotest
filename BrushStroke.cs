using Microsoft.Xna.Framework;

namespace pnt {
    internal struct BrushSetting {
        public Color color;
        public int size;
    }

    internal struct BrushStroke {
        public BrushStroke(BrushSetting setting, Vector2 position) {
            this.color = setting.color;
            this.size = setting.size;
            this.position = position;
        }

        public Color   color;
        public Vector2 position;
        public int   size; // in pixels
    }
}
