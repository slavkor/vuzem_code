package si.ismvuzem.authentication;

import si.ismvuzem.model.Token;

public interface IServerAuthenticator {
    public Token RequestAccess(final String user, final String pass, String authType) throws Exception;
}
