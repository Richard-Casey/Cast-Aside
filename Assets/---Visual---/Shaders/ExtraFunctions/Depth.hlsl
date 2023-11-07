#ifndef DEPTH
#define DEPTH

float getDepth(vec2 screen_uv, float raw_depth, mat4 inv_projection_matrix){
	vec3 normalized_device_coordinates = vec3(screen_uv * 2.0 - 1.0, raw_depth);
        vec4 view_space = inv_projection_matrix * vec4(normalized_device_coordinates, 1.0);	
	view_space.xyz = view_space.xyz / view_space.w;	
	return view_space.z;
}