package si.ismvuzem.authentication;

public class AccountGeneral {
    public static  final String BASE_URL = "http://auth.ismvuzem.si";
    public static  final String BASE_URL_DEV = "http://192.168.137.122";
    /**
     * Account type id
     */
    public static final String ACCOUNT_TYPE = "ismvuzem.si";

    /**
     * Account name
     */
    public static final String ACCOUNT_NAME = "Ism";

    /**
     * User data username
     */
    public static final String USERDATA_USER = "userName";

    /**
     * User data fields
     */
    public static final String USERDATA_USER_OBJ_ID = "userObjectId";   //Parse.com object id

    /**
     * Auth token types
     */
    public static final String AUTHTOKEN_TYPE_READ_ONLY = "Read only";
    public static final String AUTHTOKEN_TYPE_READ_ONLY_LABEL = "Read only access to an Ism account";

    public static final String AUTHTOKEN_TYPE_FULL_ACCESS = "Full access";
    public static final String AUTHTOKEN_TYPE_FULL_ACCESS_LABEL = "Full access to an Ism account";

    public static final String AUTHTOKEN_ID = "tokenId";

    public static final IServerAuthenticator sServerAuthenticate = new IsmServerAuthenticator();
}
