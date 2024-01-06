#version 410 core

in vec2 frag_texCoords;

out vec4 out_color;

uniform sampler2D uTexture;
uniform float uFilled;

void main() {
    out_color = texture(uTexture, vec2(frag_texCoords.x - uFilled, frag_texCoords.y));
}
