#version 410 core

in vec2 frag_texCoords;

out vec4 out_color;

uniform sampler2D uTexture;
uniform vec2 offset;

void main() {
    out_color = texture(uTexture, mod(frag_texCoords + offset, 1.0));
}
