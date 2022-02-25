using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon;
using Amazon.SecretsManager.Model;
using System.IO;

namespace DOR_Dashboard.Common
{
    class clsSecretManager
    {

        /*
 *      Use this code snippet in your app.
 *      If you need more information about configurations or implementing the sample code, visit the AWS docs:
 *      https://apc01.safelinks.protection.outlook.com/?url=https%3A%2F%2Faws.amazon.com%2Fdevelopers%2Fgetting-started%2Fnet%2F&amp;data=04%7C01%7Csupriyo.ghosh01%40infosys.com%7Cc98f2f21494942a9777908d977647a01%7C63ce7d592f3e42cda8ccbe764cff5eb6%7C0%7C0%7C637672095708732915%7CUnknown%7CTWFpbGZsb3d8eyJWIjoiMC4wLjAwMDAiLCJQIjoiV2luMzIiLCJBTiI6Ik1haWwiLCJXVCI6Mn0%3D%7C1000&amp;sdata=A4BBGN5WfHqWD5QN6pq9grrZtUjslfP2cCY89D%2FU%2Fo4%3D&amp;reserved=0
 *
 *      Make sure to include the following packages in your code.
 *
 *      using System;
 *      using System.IO;
 *
 *      using Amazon;
 *      using Amazon.SecretsManager;
 *      using Amazon.SecretsManager.Model;
 *
 */

        /*
         * AWSSDK.SecretsManager version="3.3.0" targetFramework="net45"
         */
        public static string GetConnectionstring(string SecretName)     
        {
            string secret = "";
            if (!SecretName.Equals("LOCAL"))
            {
                string secretName = SecretName;
                string region = "ap-southeast-1";
                //string secret = "";
                // Console.WriteLine("Started");
                MemoryStream memoryStream = new MemoryStream();
                Console.WriteLine("Reached");
                IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
                //Console.WriteLine("Reached1");
                GetSecretValueRequest request = new GetSecretValueRequest();
                //Console.WriteLine("Reached2");
                request.SecretId = secretName;
                request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

                GetSecretValueResponse response = null;

                // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
                // See https://apc01.safelinks.protection.outlook.com/?url=https%3A%2F%2Fdocs.aws.amazon.com%2Fsecretsmanager%2Flatest%2Fapireference%2FAPI_GetSecretValue.html&amp;data=04%7C01%7Csupriyo.ghosh01%40infosys.com%7Cc98f2f21494942a9777908d977647a01%7C63ce7d592f3e42cda8ccbe764cff5eb6%7C0%7C0%7C637672095708732915%7CUnknown%7CTWFpbGZsb3d8eyJWIjoiMC4wLjAwMDAiLCJQIjoiV2luMzIiLCJBTiI6Ik1haWwiLCJXVCI6Mn0%3D%7C1000&amp;sdata=QaoA5v2yv%2BYBJTHtN5UrFKR2i1R8I8PLfrd6p898F0A%3D&amp;reserved=0
                // We rethrow the exception by default.

                try
                {
                    response = client.GetSecretValueAsync(request).Result;
                    //Console.WriteLine(response);
                }
                catch (DecryptionFailureException e)
                {
                    // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                    // Deal with the exception here, and/or rethrow at your discretion.
                    //Console.WriteLine("Decrypfail");
                    //Console.WriteLine(e.Message);


                }
                catch (InternalServiceErrorException e)
                {
                    // An error occurred on the server side.
                    // Deal with the exception here, and/or rethrow at your discretion.
                    //Console.WriteLine("ISE");
                    //Console.WriteLine(e.Message);
                }
                catch (InvalidParameterException e)
                {
                    // You provided an invalid value for a parameter.
                    // Deal with the exception here, and/or rethrow at your discretion

                }
                catch (InvalidRequestException e)
                {
                    // You provided a parameter value that is not valid for the current state of the resource.
                    // Deal with the exception here, and/or rethrow at your discretion.



                }
                catch (ResourceNotFoundException e)
                {
                    // We can't find the resource that you asked for.
                    // Deal with the exception here, and/or rethrow at your discretion.
                    //Console.WriteLine("RNF");
                    //Console.WriteLine(e.Message);

                }
                catch (System.AggregateException e)
                {
                    // More than one of the above exceptions were triggered.
                    // Deal with the exception here, and/or rethrow at your discretion.
                    //Console.WriteLine(e.InnerException.Message);
                    //Console.WriteLine(e.Message);
                    //Console.ReadLine();

                }

                // Decrypts secret using the associated KMS CMK.
                // Depending on whether the secret is a string or binary, one of these fields will be populated.
                if (response.SecretString != null)
                {
                    secret = response.SecretString;

                    //obtained the secret 
                    //form the required string 

                    //Console.WriteLine("Decoded binary secret " + secret);
                }
                else
                {
                    memoryStream = response.SecretBinary;
                    StreamReader reader = new StreamReader(memoryStream);
                    string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                    secret = decodedBinarySecret;
                    //Console.WriteLine("Decoded binary secret " + decodedBinarySecret);
                }
                // Console.ReadLine();
                if (!secret.Equals(string.Empty))
                {

                    Secret objdtls = Newtonsoft.Json.JsonConvert.DeserializeObject<Secret>(secret);
                    secret = string.Format("Data source={0},{4};Initial Catalog={1};User ID={2};Password={3};Connection Timeout=3000", objdtls.host, objdtls.dbname, objdtls.username,objdtls.password, objdtls.port);
                }
                //form the connection string 
                // string.Format("Data source={0};Initial Catalog={1};User ID={2};Password={3};Connection Timeout=3000", objdtls.host,’db Name required here ’, objdtls.username, objdtls.password);
                return secret;
            }

            else
            {
                //secret = "{""username"":""sa"",""password"":""sqlserversa@1"",""engine"":""sqlserver"",""host"":""SGSGPCCW - L44225\SQLEXPRESS"",""port"":1450,""dbInstanceIdentifier"":""seaawsadvp411"",""dbname"":""FORMZU""}";
                //secret = "{""username"";//:""sa"",""password"":""sqlserversa@1"",""engine"":""sqlserver"",""host"":""SGSGPCCW - L44225\SQLEXPRESS"",""port"":1450,""dbInstanceIdentifier"":""seaawsadvp411"",""dbname"":""FORMZU""}";

                secret = "{ \"username\":\"sa\",\"password\":\"sqlserversa@1\",\"engine\":\"sqlserver\",\"host\":\"155.14.156.10\",\"port\":1450,\"dbInstanceIdentifier\":\"seaawsadvp411\",\"dbname\":\"FORMZU\"}";
                Secret objdtls = Newtonsoft.Json.JsonConvert.DeserializeObject<Secret>(secret);
                objdtls.host = "SGSGPCCW-L44225\\SQLEXPRESS";
                //secret = string.Format("Data source={0};Initial Catalog={1};User ID={2};Password={3};Connection Timeout=3000", objdtls.host, objdtls.dbname, objdtls.username, objdtls.password);
                secret = string.Format("Data source={0};Initial Catalog={1};User ID={2};Password={3};Connection Timeout=3000", objdtls.host, objdtls.dbname, objdtls.username, objdtls.password, objdtls.port);

                return secret;
            }
        }
    }
    
}
