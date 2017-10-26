export const createApiUrl = (route:string, method:string, data:string = ''):string => (
    `${process.env.REACT_APP_API_URL}/${route}/${method}/${data}`
)

export const createRequestOptions = (method:string, body:any = null):RequestInit => (
    {
        headers: new Headers({
            "user-key": process.env.REACT_APP_API_ACCESS_KEY,
            "Content-Type": "application/json"
        }),
        method: method,
        body: body == null ? null : JSON.stringify(body),
        mode: 'cors'
    }
)