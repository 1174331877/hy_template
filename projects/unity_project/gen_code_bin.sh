#!/bin/zsh

GEN_CLIENT=luban/Tools/Luban.ClientServer/Luban.ClientServer.dll
CONF_ROOT=cfgs

dotnet ${GEN_CLIENT} -j cfg --\
 -d ${CONF_ROOT}\Defines\__root__.xml \
 --input_data_dir ${CONF_ROOT}\Datas \
 --output_code_dir tower-defense/Assets/GenCfgs \
 --output_data_dir tower-defense/Assets/Res/BundleRes/cfgs/bytes \
 --gen_types code_cs_bin,data_bin \
 -s all 


