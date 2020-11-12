using System.Text.Json;

namespace gamitude_backend.Dto
{
    public class ControllerResponse<T>
    {
        public T data { get; set; }

        public bool success { get; set; } = true;
    }

    public class ControllerErrorResponse
    {
        /// <summary>
        /// succes flag
        /// </summary>
        /// <example>false</example>
        public bool success { get; set; } = false;

        /// <summary>
        /// Error message
        /// </summary>
        /// <example>Error message</example>
        public string message { get; set; } = null;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}