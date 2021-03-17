/*
    E621 API Wrapper
    Written by Epsi Rho
    Version 0.1
    Last Updated 0.1 03/17/21@1:07
*/

namespace e6API.Enums
{
    public enum StatusCode
    {
        MissingOrInvalidParameter=1,
        JsonConversionFailure=2,
        Forbidden=403,
        NotFound=404,
        PreconditionFailed=412,
        InvalidRecord=420,
        UserThrottled=421,
        Locked=422,
        AlreadExists=423,
        InvalidParameters=424,
        InternalServerError=500,
        BadGateway=502,
        ServiceUnavailable=503,
        UnknownError=520,
        CloudFlareConnectionTimeOut=522,
        OriginConnectionTimeOut=524,
        SSLHandshakeFailed=525
    }
}
