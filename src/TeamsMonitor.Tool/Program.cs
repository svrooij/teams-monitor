using System.CommandLine;
using System.Reflection;

WriteHeader();
var rootCommand = new MonitorCommand();
// The minimal template has no access to 'args'
// and Environment.GetCommandLineArgs() has the application as first parameter
return await rootCommand.InvokeAsync(Environment.GetCommandLineArgs().Skip(1).ToArray());

const string header = @",---------.    .-''-.     ____    ,---.    ,---.   .-'''-.                              
\          \ .'_ _   \  .'  __ `. |    \  /    |  / _     \                             
 `--.  ,---'/ ( ` )   '/   '  \  \|  ,  \/  ,  | (`' )/`--'                             
    |   \  . (_ o _)  ||___|  /  ||  |\_   /|  |(_ o _).                                
    :_ _:  |  (_,_)___|   _.-`   ||  _( )_/ |  | (_,_). '.                              
    (_I_)  '  \   .---..'   _    || (_ o _) |  |.---.  \  :                             
   (_(=)_)  \  `-'    /|  _( )_  ||  (_,_)  |  |\    `-'  |                             
    (_I_)    \       / \ (_ o _) /|  |      |  | \       /                              
    '---'     `'-..-'   '.(_,_).' '--'      '--'  `-...-'                               
,---.    ,---.    ,-----.    ,---.   .--..-./`) ,---------.    ,-----.    .-------.     
|    \  /    |  .'  .-,  '.  |    \  |  |\ .-.')\          \ .'  .-,  '.  |  _ _   \    
|  ,  \/  ,  | / ,-.|  \ _ \ |  ,  \ |  |/ `-' \ `--.  ,---'/ ,-.|  \ _ \ | ( ' )  |    
|  |\_   /|  |;  \  '_ /  | :|  |\_ \|  | `-'`""`    |   \  ;  \  '_ /  | :|(_ o _) /    
|  _( )_/ |  ||  _`,/ \ _/  ||  _( )_\  | .---.     :_ _:  |  _`,/ \ _/  || (_,_).' __  
| (_ o _) |  |: (  '\_/ \   ;| (_ o _)  | |   |     (_I_)  : (  '\_/ \   ;|  |\ \  |  | 
|  (_,_)  |  | \ `""/  \  ) / |  (_,_)\  | |   |    (_(=)_)  \ `""/  \  ) / |  | \ `'   / 
|  |      |  |  '. \_/``"".'  |  |    |  | |   |     (_I_)    '. \_/``"".'  |  |  \    /  
'--'      '--'    '-----'    '--'    '--' '---'     '---'      '-----'    ''-'   `'-'   
                                                                                        ";

void WriteHeader() {
    Console.WriteLine(header);
    Console.WriteLine();
    Console.WriteLine("Teams monitor v{0} by @svrooij", System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version);
    Console.WriteLine("Docs and Source: https://github.com/svrooij/teams-monitor");
    Console.WriteLine();

}