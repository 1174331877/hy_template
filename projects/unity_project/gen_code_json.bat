set PROJECT_NAME= project_template
set GEN_CLIENT= luban\Tools\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT= cfgs

%GEN_CLIENT% -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_code_dir %PROJECT_NAME%/Assets/GenCfgs ^
 --output_data_dir %PROJECT_NAME%/Assets/Res/BundleRes/cfgs\jsons ^
 --gen_types code_cs_unity_json,data_json ^
 -s all 

pause