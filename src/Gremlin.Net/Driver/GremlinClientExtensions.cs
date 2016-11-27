#region License

/*
 * Copyright 2016 Florian Hockmann
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gremlin.Net.Driver.Messages;

namespace Gremlin.Net.Driver
{
    /// <summary>
    /// Provides extension methods for the <see cref="IGremlinClient"/> interface.
    /// </summary>
    public static class GremlinClientExtensions
    {
        /// <summary>
        /// Submits a request message that consists of a script with bindings as an asynchronous operation where only a single result gets returned.
        /// </summary>
        /// <remarks>If multiple results are received from Gremlin Server, then only the first gets returned. Use <see cref="SubmitAsync{T}"/> instead when you expect a collection of results.</remarks>
        /// <typeparam name="T">The type of the expected result.</typeparam>
        /// <param name="gremlinClient">The <see cref="IGremlinClient"/> that submits the request.</param>
        /// <param name="requestScript">The Gremlin request script to send.</param>
        /// <param name="bindings">Bindings for parameters used in the requestScript.</param>
        /// <returns>A single result received from the Gremlin Server.</returns>
        /// <exception cref="Exceptions.ResponseException">Thrown when a response is received from Gremlin Server that indicates that an error occurred.</exception>
        public static async Task<T> SubmitWithSingleResultAsync<T>(this IGremlinClient gremlinClient,
            string requestScript,
            Dictionary<string, object> bindings = null)
        {
            var resultCollection = await gremlinClient.SubmitAsync<T>(requestScript, bindings).ConfigureAwait(false);
            return resultCollection.FirstOrDefault();
        }

        /// <summary>
        /// Submits a request message as an asynchronous operation where only a single result gets returned.
        /// </summary>
        /// <remarks>If multiple results are received from Gremlin Server, then only the first gets returned. Use <see cref="SubmitAsync{T}"/> instead when you expect a collection of results.</remarks>
        /// <typeparam name="T">The type of the expected result.</typeparam>
        /// <param name="gremlinClient">The <see cref="IGremlinClient"/> that submits the request.</param>
        /// <param name="requestMessage">The <see cref="ScriptRequestMessage"/> to send.</param>
        /// <returns>A single result received from the Gremlin Server.</returns>
        /// <exception cref="Exceptions.ResponseException">Thrown when a response is received from Gremlin Server that indicates that an error occurred.</exception>
        public static async Task<T> SubmitWithSingleResultAsync<T>(this IGremlinClient gremlinClient,
            ScriptRequestMessage requestMessage)
        {
            var resultCollection = await gremlinClient.SubmitAsync<T>(requestMessage).ConfigureAwait(false);
            return resultCollection.FirstOrDefault();
        }

        /// <summary>
        /// Submits a request message that consists of a script with bindings as an asynchronous operation without returning the result received from the Gremlin Server.
        /// </summary>
        /// <param name="gremlinClient">The <see cref="IGremlinClient"/> that submits the request.</param>
        /// <param name="requestScript">The Gremlin request script to send.</param>
        /// <param name="bindings">Bindings for parameters used in the requestScript.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.ResponseException">Thrown when a response is received from Gremlin Server that indicates that an error occurred.</exception>
        public static async Task SubmitAsync(this IGremlinClient gremlinClient, string requestScript,
            Dictionary<string, object> bindings = null)
        {
            await gremlinClient.SubmitAsync<object>(requestScript, bindings).ConfigureAwait(false);
        }

        /// <summary>
        /// Submits a request message as an asynchronous operation without returning the result received from the Gremlin Server.
        /// </summary>
        /// <param name="gremlinClient">The <see cref="IGremlinClient"/> that submits the request.</param>
        /// <param name="requestMessage">The <see cref="ScriptRequestMessage"/> to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.ResponseException">Thrown when a response is received from Gremlin Server that indicates that an error occurred.</exception>
        public static async Task SubmitAsync(this IGremlinClient gremlinClient, ScriptRequestMessage requestMessage)
        {
            await gremlinClient.SubmitAsync<object>(requestMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Submits a request message that consists of a script with bindings as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The type of the expected results.</typeparam>
        /// <param name="gremlinClient">The <see cref="IGremlinClient"/> that submits the request.</param>
        /// <param name="requestScript">The Gremlin request script to send.</param>
        /// <param name="bindings">Bindings for parameters used in the requestScript.</param>
        /// <returns>An enumerable collection of the data returned from the server.</returns>
        /// <exception cref="Exceptions.ResponseException">Thrown when a response is received from Gremlin Server that indicates that an error occurred.</exception>
        public static async Task<IEnumerable<T>> SubmitAsync<T>(this IGremlinClient gremlinClient, string requestScript,
            Dictionary<string, object> bindings = null)
        {
            var requestMessage = new ScriptRequestMessage
            {
                Arguments = new ScriptRequestArguments {GremlinScript = requestScript, Bindings = bindings}
            };
            return await gremlinClient.SubmitAsync<T>(requestMessage).ConfigureAwait(false);
        }

        public static IEnumerable<T> Submit<T>(this IGremlinClient gremlinClient, ScriptRequestMessage requestMessage)
        {
            return gremlinClient.SubmitAsync<T>(requestMessage).Result;
        }
    }
}